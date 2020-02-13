namespace SIS.MvcFramework
{
    using SIS.MvcFramework.Validation;

    public class ControllerState : IControllerState
    {
        public ControllerState()
        {
            this.Reset();
        }

        public ModelStateDictionary ModelState { get; set; }

        public void Reset()
        {
            this.ModelState = new ModelStateDictionary();
        }

        public void Initialize(Controller controller)
        {
            this.ModelState = controller.ModelState;
        }

        public void SetState(Controller controller)
        {
            controller.ModelState = this.ModelState;
        }
    }
}
