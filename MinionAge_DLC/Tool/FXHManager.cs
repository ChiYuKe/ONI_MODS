using UnityEngine;

namespace OxygenConsumingPlant.Tool
{
  //特效生成
    public static class KFXHManager
    {

        //public static void FXH(Vector3 position, Transform parent = null, string anim_file_name = "moo_call_fx_kanim", bool a = false, string PlayName = "moo_call")
        //{
        //    KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(anim_file_name, position, parent, false, Grid.SceneLayer.Front, false);
        //    if (a) 
        //    {
        //        kbatchedAnimController.destroyOnAnimComplete = true;
        //        kbatchedAnimController.Play(PlayName, KAnim.PlayMode.Once, 2f, 0f);
        //    }
            
        //}
        public static void FXH(Vector3 position, Transform parent = null)
        {
            KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("upgrade_fx_kanim", position, parent, false, Grid.SceneLayer.Front, false);
            
        }
    }
}