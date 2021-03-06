using UnityEngine;

public class IdleState : IAState {

    private EventAction nextAction;
    public float time = -1;
    private float timeRemaining;
    public bool isInfinite = true;

    public override void start(GameObject obj) {
        base.start(obj);
        this.timeRemaining = time;
    }

    public IdleState onComplete(EventAction nextAction) {
        this.nextAction = nextAction;
        return this;
    }

    public override IAState update(GameObject obj) {
        if (!isInfinite  ) {

            timeRemaining -= Time.deltaTime;

            if (timeRemaining < 0) {
                return nextAction == null ? null : nextAction.Invoke(from);
            }
        }
        if (animatorController != null) {
            animatorController.setForward(0);
        }
        return this;
    }

    public override int getCod() {
        return 5;
    }


}