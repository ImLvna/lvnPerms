
using UdonSharp;

namespace gay.lvna.common.udon
{
    public class Colored : UdonSharpBehaviour
    {
        public static string C(string text, string color)
        {
            return "<color=" + color + ">" + text + "</color>";
        }
    }
}