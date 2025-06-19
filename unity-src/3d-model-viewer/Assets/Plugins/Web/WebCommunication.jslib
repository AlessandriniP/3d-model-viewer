mergeInto(LibraryManager.library, {
  SendMessageToWeb: function(messagePtr) {
    var message = UTF8ToString(messagePtr);

    if (typeof window.onUnityMessage === "function") {
      window.onUnityMessage(message);
    }
  },
});
