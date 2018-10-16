// Copyright 2018 Ruben Buniatyan
// Licensed under the MIT License. For full terms, see LICENSE in the project root.

using System;
using Android.Content;

namespace Macaron.Content
{
    public class SimpleBroadcastReceiver : BroadcastReceiver
    {
        private readonly Action<Intent> _callback;

        public SimpleBroadcastReceiver(Action<Intent> callback) => _callback = callback ?? throw new ArgumentNullException(nameof(callback));

        public override void OnReceive(Context context, Intent intent) => _callback(intent);
    }
}
