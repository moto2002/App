/// The modified version of this software is Copyright (C) 2013 ZHing.
/// The original version's copyright as below.

/* Copyright (C) 2012 Ruslan A. Abdrashitov

Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions 
of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE. */

namespace HTMLEngine.Core
{
    internal class HtmlChunkWord : HtmlChunk
    {
        public string Text;

        public bool ReadWord(Reader reader)
        {
            reader.AutoSkipWhitespace = false;
            this.Text = reader.ReadToWhitespaceOrChar('<');
            if (string.IsNullOrEmpty(this.Text) == false)
            {
                this.Text = this.Text.Replace("&nbsp;", " ")
                    .Replace("&gt;", ">")
                    .Replace("&lt;", "<")
                    ;
                return true;
            }
            return false;
        }

        public override string ToString() { return string.Format("WORD:" + this.Text); }
    }
}