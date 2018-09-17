namespace Data.ViewModels.Modal
{
    public class Modal
    {
        public string Id { get; set; }
        public string AriaLabelId { get; set; }
        public ModalSize Size { get; set; }

        public string ModalSizeClass
        {
            get
            {
                switch (this.Size)
                {
                    case ModalSize.Small:
                        return "modal-sm";
                    case ModalSize.Large:
                        return "modal-lg";
                    case ModalSize.Medium:
                        return "modal-md";
                    default:
                        return "";
                }
            }
        }
    }
}
