using System.Collections.Generic;

namespace KHCN.Data.Helpers
{
    public class JSTree
    {
        public JSTree()
        {
            icon = "fa fa-cubes";
            state = new JSTreeState();
        }
        public int id { get; set; }
        public string text { get; set; }
        public JSTreeState state { get; set; }
        public string icon { get; set; }
        public List<JSTree> children { get; set; }
    }

    public class JSTreeState
    {
        public JSTreeState()
        {
            opened = true;
            disabled = false;
            selected = false;
        }
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }

    public class AttachmentUpload
    {
        public AttachmentUpload()
        {
            RelativePath = "";
            Keyword = "";
            FileName = "";
            DisplayName = "";
            FileSize = "";
            ApsolutePath = "";
        }
        public string RelativePath { get; set; }
        public string Keyword { get; set; }
        public string FileName { get; set; }
        public string DisplayName { get; set; }
        public string FileSize { get; set; }
        public string ApsolutePath { get; set; }
    }

    public class ControllerActions
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
    }
}
