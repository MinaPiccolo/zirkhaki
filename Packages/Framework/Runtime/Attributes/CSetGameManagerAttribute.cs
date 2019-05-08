/*
 * With this attribute game systems can specify their game manager class.
 * Each game system class needs to set their respective game manager class otherwise can not instantiated.
 * 
 * Author: Ideen Molavi, ideenmolavi@gmail.com
 * Creation time: 10-Oct-2017, Sprint 4
 */

using System;

[AttributeUsage(AttributeTargets.Class)]
public class CSetGameManagerAttribute : Attribute
{
    private Type _gameManagerType;
    public CSetGameManagerAttribute(Type gameManager)
    {
        _gameManagerType = gameManager;
    }

    public Type GetGameManagerType()
    {
        return _gameManagerType;
    }
}
