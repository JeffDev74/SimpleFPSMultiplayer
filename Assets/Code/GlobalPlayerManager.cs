using UnityEngine;
using UnityEngine.Networking;

namespace FPS
{
    //public struct Foo
    //{
    //    int bar;

    //    public Foo(int a)
    //    {
    //        bar = a;
    //    }
    //}

    //public class FooSync : SyncListStruct<Foo> { }
    ////public class SyncListPlayerUint : SyncListUInt { }

    public class GlobalPlayerManager : NetworkBehaviour
    {
        //public FooSync sync = new FooSync();
        //   public SyncListString syncedPlayerList = new SyncListString();

        //public void AddPlayerToList(uint playerUintID)
        //{
        //    syncedPlayerList.Add(playerUintID.ToString());
        //}

        //void Update()
        //{
        //    Debug.Log("The list count is ["+syncedPlayerList.Count+"]");
        //}
    }
}