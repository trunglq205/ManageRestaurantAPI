namespace QLNhaHang.API.Exceptions
{
    public class QLNhaHangException : Exception
    {
        string? MsgError = null;
        public QLNhaHangException(string msg)
        {
            this.MsgError = msg;
        }
        public override string Message
        {
            get { return MsgError; }
        }
    }
}
