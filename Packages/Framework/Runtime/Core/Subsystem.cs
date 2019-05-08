using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Revy.Framework
{
    public sealed partial class MFramework
    {
        private static class Subsystem
        {
            private static readonly List<ISubsystem> _subsystems = new List<ISubsystem>();

            public static async Task Setup()
            {
                float oldTime = Time.realtimeSinceStartup;
                InstantiateSubsystems();
                await Initialization.InitializeAsync(_subsystems);
                CLog.Log($"Loading Subsystems takes {Time.realtimeSinceStartup - oldTime} seconds.");
            }

            public static void Add(ISubsystem subsystem)
            {
                if (!_subsystems.Contains(subsystem))
                    _subsystems.Add(subsystem);
            }

            public static void Remove(ISubsystem subsystem)
            {
                _subsystems.Remove(subsystem);
            }
            
            public static void Reset()
            {
                _subsystems.Clear();
            }
            
            private static void InstantiateSubsystems()
            {
                Type[] allSubsystems = CUtilities.GetAllImplementingTypes(typeof(ISubsystem));

                if (allSubsystems == null)
                {
                    CLog.Log(" Can not find any subsystems in this assembly when trying to instantiate subsystems.",
                        category: LOG_TAG);
                    return;
                }

                int count = allSubsystems.Length;
                for (int i = 0; i < count; i++)
                {
                    Type subsystem = allSubsystems[i];

                    if (subsystem.IsDefined(typeof(CDisableAutoInstantiationAttribute), false)) continue;
                    if (subsystem.IsAbstract) continue;
                    if (!_config.IsSubsystemEnable(subsystem)) continue;

                    if (subsystem.IsSubclassOf(typeof(FComponent)))
                    {
                        Persistent.Instantiate(subsystem, subsystem.Name, PersistentSubCategories.SUBSYSTEMS);
                    }
                    else
                    {
                        Activator.CreateInstance(subsystem);
                    }
                }
            }
        }
    }
}