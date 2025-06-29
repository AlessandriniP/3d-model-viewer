mergeInto(LibraryManager.library, {
    ModelsFetched: function(paramPtr) {
    var param = UTF8ToString(paramPtr);

    if (typeof window.onUnityMessage === "function") {
      window.onUnityMessage(param);
    }
  },

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

  ObjectDescription: function(paramPtr, value1Ptr, value2Ptr) {
    var param = UTF8ToString(paramPtr);
    var value1 = UTF8ToString(value1Ptr);
    var value2 = UTF8ToString(value2Ptr);

    if (typeof window.onUnityMessage === "function") {
      window.onUnityMessage(param, value1, value2);
    }
  },
});
