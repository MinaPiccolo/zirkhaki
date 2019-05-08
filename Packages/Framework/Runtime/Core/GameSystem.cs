using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Revy.Framework
{
    public sealed partial class MFramework
    {
        private static class GameSystem
        {
            /// <summary>
            /// Contains list of all game systems that instantiated.
            /// </summary>
            private static readonly List<IGameSystem> _gameSystems = new List<IGameSystem>();

            public static async Task Setup()
            {
                var oldTime = Time.realtimeSinceStartup;

                SetupGameManager();
                InstantiateGameSystems();
                await InitializeGameSystems();

                CLog.Log($"Loading Game Systems takes {Time.realtimeSinceStartup - oldTime} seconds.",
                    category: LOG_TAG);
            }

            public static void Add(IGameSystem gameSystem)
            {
                if (!_gameSystems.Contains(gameSystem))
                    _gameSystems.Add(gameSystem);
            }

            public static void Remove(IGameSystem gameSystem)
            {
                _gameSystems.Remove(gameSystem);
            }

            public static void Reset()
            {
                _gameSystems.Clear();
            }

            private static void SetupGameManager()
            {
                var frameworkConfig = _config;
                if (frameworkConfig == null) return;

                // Cache GameManagerType to maintain performance. GameManagerType takes a lot of memory and CPU time.
                var gameManagerType = frameworkConfig.GameManagerType;
                if (gameManagerType == null)
                {
                    CLog.Log(
                        "Can not create <b>Game Manager</b> because type of Game Manager does not specified in frameworks configurations.",
                        category: LOG_TAG);
                    return;
                }

                if (typeof(FComponent).IsAssignableFrom(gameManagerType))
                {
                    var existingGameMangers = FindObjectsOfType(gameManagerType);
                    for (int i = 0; i < existingGameMangers.Length; i++)
                    {
                        DestroyImmediate(existingGameMangers[i]);
                    }
                    GameManager = Persistent.Instantiate(gameManagerType,
                        subcategory: gameManagerType.Name, parentName: "GameSystems") as IGameManager;
                }
                else
                {
                    GameManager = Activator.CreateInstance(gameManagerType) as IGameManager;
                }

                if (GameManager == null)
                {
                    CLog.Error($"Can not create game manger component.", category: LOG_TAG);
                    return;
                }

                CLog.Log(
                    $"Game Manager Class(<b>{gameManagerType}</b>) successfully instantiated.", category: LOG_TAG);
            }

            private static void InstantiateGameSystems()
            {
                Type[] gameSystems = CUtilities.GetAllImplementingTypes(typeof(IGameSystem));
                if (gameSystems == null || gameSystems.Length == 0) return;

                int gameSystemsCount = gameSystems.Length;
                for (int i = 0; i < gameSystemsCount; i++)
                {
                    Type gameSystem = gameSystems[i];
                    if (!IsValidGameSystem(gameSystem)) continue;
                    if (gameSystem.IsSubclassOf(typeof(FComponent)))
                    {
                        Persistent.Instantiate(gameSystem, subcategory: gameSystem.Name,
                            parentName: "GameSystems");
                    }
                    else
                    {
                        Activator.CreateInstance(gameSystem);
                    }
                }
            }

            /// <summary>
            /// Invokes Initialize(),BeginPlay() on registered IFObjects.
            /// First subsystem will initialized then other objects.
            /// Will invoke in Start().
            /// </summary>
            /// <returns></returns>
            private static async Task InitializeGameSystems()
            {
                if (GameManager == null)
                {
                    IsPlayBegun = false;
                    return;
                }

                await Initialization.InitializeAsync(_gameSystems);
            }


            private static bool IsValidGameSystem(Type gameSystem)
            {
                if (gameSystem == null) return false;

                if (gameSystem.IsInterface || gameSystem.IsAbstract) return false;

                if (IsGameManager(gameSystem)) return false;

                if (HasDisableInstantiationAttribute(gameSystem)) return false;

                if (!HasCorrectGameManagerAttribute(gameSystem)) return false;

                if (!_config.IsGameSystemEnable(gameSystem)) return false;

                return true;
            }

            private static bool HasDisableInstantiationAttribute(Type gameSystem)
            {
                return gameSystem.IsDefined(typeof(CDisableAutoInstantiationAttribute));
            }

            private static bool IsGameManager(Type gameSystem)
            {
                return typeof(IGameManager).IsAssignableFrom(gameSystem);
            }

            private static bool HasCorrectGameManagerAttribute(Type gameSystem)
            {
                CSetGameManagerAttribute gameManagerAttribute =
                    gameSystem.GetCustomAttribute(typeof(CSetGameManagerAttribute)) as CSetGameManagerAttribute;

                if (gameManagerAttribute == null)
                {
#if LOG_WARNING
                    Debug.LogWarning(
                        $"{LOG_TAG} '{gameSystem.Name}' does not have 'CSetGameManager' attribute. All game systems must have this attribute.");
#endif
                    return false;
                }

                Type gameManager = gameManagerAttribute.GetGameManagerType();

                if (gameManager == null)
                {
#if LOG_ERROR
                    Debug.LogError(
                        $"{LOG_TAG} Constructor argument of CSetGameMnager in '{gameSystem.Name}' is null. ");
#endif
                    return false;
                }

                if (_config == null || _config.GameManagerType == null) return false;

                if (gameManager != _config.GameManagerType)
                {
                    //Debug.Log($"<b>{gameSystem.Name}</b> game system has <b>{gameManager.Name}</b> game manager type and does not instantiated. Current game manager is <b>{Config.GameManagerType.Name}</b>.");
                    return false;
                }

                return true;
            }
        }
    }
}