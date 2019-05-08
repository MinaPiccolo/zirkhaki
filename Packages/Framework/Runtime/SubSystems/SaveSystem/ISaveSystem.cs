/**
 * Author: Mahdi Fada
 * CreationTime: 11 / 5 / 2017
 * Description: Base interface for file system
 **/
namespace Revy.Framework
{
    public interface ISaveSystem
    {
        void Save<T>(T item, string fileName);
        T Load<T>(string fileName);
    }
}
