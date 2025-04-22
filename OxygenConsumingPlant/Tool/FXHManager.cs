using UnityEngine;

namespace OxygenConsumingPlant.Tool
{
    // Token: 0x0200000F RID: 15
    public static class FXHManager
    {
        // Token: 0x06000041 RID: 65 RVA: 0x00003E58 File Offset: 0x00002058
        public static void FXH(Vector3 position)
        {
            KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("moo_call_fx_kanim", position, null, false, Grid.SceneLayer.Front, false);
            kbatchedAnimController.destroyOnAnimComplete = true;
            kbatchedAnimController.Play("moo_call", KAnim.PlayMode.Once, 2f, 0f);
        }
    }
}