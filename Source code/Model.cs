using System.Text.RegularExpressions;

namespace Synchronizer_MVP_pattern
{

    /*-----------------------------------------------------------------*/

    public abstract class Model
    {

        /*-----------------------------------------------------------------*/

        public enum LogType
        {
                Create = 0
            ,   Rename
            ,   Delete
            ,   Common
            ,   Count
        }

        public enum ModelPart
        {
            Client = 0, Server = 1
        }

        /*-----------------------------------------------------------------*/

        public readonly static int FIXED_BUFFER_SIZE = 3;

        public readonly static int MAX_BYTES_BUFFER_SIZE = 2048;

        public readonly static Regex PortRegex = new Regex(@"\D");

        /*-----------------------------------------------------------------*/

    }

    /*-----------------------------------------------------------------*/

}
