using Assets.LFramework.Core;
using UnityEngine;

namespace Assets
{
    public class Main : MonoBehaviour {
        void Start()
        {
            AppFacade.Instance().InitStart();           
        }
    }
}
