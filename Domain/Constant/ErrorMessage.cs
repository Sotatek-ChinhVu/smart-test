﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constant
{
    public static class ErrorMessage
    {
        public const string MessageType_mChk00020 = "{0}が有効期限切れです。" + "\r\n" + "有効期限を確認してください。";
        public const string MessageType_mChk00080 = "{0}" + "\r\n" + "{1}を確認してください。";
        public const string MessageType_mChk00030 = "{0}が未確認です。" + "\r\n" + "{1}を確認してください。";
        public const string MessageType_mNG01010 = "{0}が正しくありません。";
        public const string MessageType_mInp00011 = "{0}の{1}を入力してください。";
        public const string MessageType_mSel01010 = "{0}を選択してください。";
        public const string MessageType_mInp00041 = "{0}は {1}を入力してください。";
    }
}
