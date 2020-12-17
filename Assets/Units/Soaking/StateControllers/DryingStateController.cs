using System.Collections;
using Units.StateManagement;
using UnityEngine;

namespace Units.Soaking.StateControllers
{
    public class DryingStateController : AbstractStateController
    {
        private Coroutine coroutine;
        private new readonly SoakingStateManager context;

        public DryingStateController(SoakingStateManager context) : base(context)
        {
            this.context = context;
        }

        public override void Execute()
        {
            if (coroutine == null)
            {
                coroutine = context.StartCoroutine(Soak());
            }
        }

        private IEnumerator Soak()
        {
            for (var i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(context.soakingTime / 10f);
                context.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.blue, Color.white, (i + 1f) / 10f);
            }

            context.State = SoakingState.None;
            coroutine = null;
        }
    }
}