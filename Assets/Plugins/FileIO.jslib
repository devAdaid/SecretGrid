// https://gist.github.com/robertwahler/b3110b3077b72b4c56199668f74978a0

var FileIO = {

  SaveToLocalStorage : function(key, data) {
    localStorage.setItem(UTF8ToString(key), UTF8ToString(data));
  },

  LoadFromLocalStorage : function(key) {
    var returnStr = localStorage.getItem(UTF8ToString(key));
    if (returnStr) {
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    } else {
        return null;
    }
  },

  RemoveFromLocalStorage : function(key) {
    localStorage.removeItem(UTF8ToString(key));
  },

  HasKeyInLocalStorage : function(key) {
    if (localStorage.getItem(UTF8ToString(key))) {
      return 1;
    }
    else {
      return 0;
    }
  }
};

mergeInto(LibraryManager.library, FileIO);