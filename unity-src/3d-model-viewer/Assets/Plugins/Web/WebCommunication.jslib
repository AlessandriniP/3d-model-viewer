mergeInto(LibraryManager.library, {
  CanShowPreviousObject: function(paramPtr, value) {
    var param = UTF8ToString(paramPtr);

    if (typeof window.onUnityMessage === "function") {
      window.onUnityMessage(param, value);
    }
  },

  CanShowNextObject: function(paramPtr, value) {
    var param = UTF8ToString(paramPtr);

    if (typeof window.onUnityMessage === "function") {
      window.onUnityMessage(param, value);
    }
  },
});
