using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController), typeof(BasicObjectAttr))]
public class FSMManager : MonoBehaviour, IEventSystemHandler {


    protected List<IAState> states = new List<IAState>();
    protected IAState currentState;
    protected BasicState basicState;

    private static int MAX_MEM_STATES = 5;
    private BasicObjectAttr attributes;

    void Awake() {
        basicState = new BasicState(this);
        basicState.start(this.gameObject);
        attributes = GetComponent<BasicObjectAttr>();
    }

    public FSMManager() {
    }

    public void setCurrentState(IAState next) {
        IAState previous = null;
        if (next == null) {
            this.currentState = null;
            return;
        }

        // same state, ignore
        if (this.currentState != null && next.getCod() == this.currentState.getCod()) {
            return;
        }

        if (states.Count >= MAX_MEM_STATES) {
            states.Remove(states[0]);
        }

        if (this.currentState != null) {
            states.Add(this.currentState);
            previous = this.currentState;
        }

        this.currentState = next;
        this.currentState.setPrevious(previous);

        // Initialize the state.
        this.currentState.start(gameObject);
    }

    public IAState GetCurrentState(){
        return this.currentState;
    }

    public void finishCurrentState() {

        // No behavior?
        if (states.Count == 0) {
            this.currentState = null;
            return;
        }

        states.Remove(currentState);

        if (states.Count > 0) {
            this.currentState = states[states.Count - 1];
        }

        if (this.currentState != null) {
            this.currentState.start(gameObject);
        }

    }

    void Update() {


        // Always invoke basic state.
        this.basicState.update(gameObject);

        // No behavior?
        if (currentState != null) {
            IAState next = this.currentState.update(gameObject);

            // This state was finished?
            if (next == null) {
                finishCurrentState();
            } else if (next != this.currentState) {
                // The state was updated?
                // Then use the new state, and add to List
                setCurrentState(next);
            }
        }


        //        Debug.Log("Alive?" + attributes.isAlive().GetValueOrDefault(true));

        if (!attributes.isAlive().GetValueOrDefault(true)) {
            currentState = null;
            // Object was killed.
            //            gameObject.Send<IAnimalAnimatorHelper>((_ => _.die()));
        }
    }

    public BasicState GetBasicState() {
        return basicState;
    }
}