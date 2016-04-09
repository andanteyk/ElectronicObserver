using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SwfDotNet.IO;
using SwfDotNet.IO.Tags;

namespace ElectronicObserver.Utility
{
    public static class SwfDecompiler
    {
        public static string GetSoundFile(string SwfFileName, string soundName)
        {
            SwfReader swfReader = new SwfReader(SwfFileName, true);
            var swf = swfReader.ReadSwf();
            var tagsEnu = swf.Tags.GetEnumerator(); //Browse swf tags list
            while (tagsEnu.MoveNext())
            {
                var tag = (BaseTag)tagsEnu.Current;
                if (tag is DefineSoundTag) //Extract a sound file:
                {
                    var soundTag = (DefineSoundTag)tag;
                    string outfileName = Path.GetTempPath() + "\\" + soundName;
                    if (soundTag.SoundFormat == SoundCodec.MP3)
                        outfileName += ".mp3";
                    else
                        outfileName += ".wav";
                    soundTag.DecompileToFile(outfileName);
                    return outfileName;
                }
            }
            return null;
        }
    }
}
