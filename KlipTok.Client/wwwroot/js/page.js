(function() {

  const page = {

    setTitle : function(newTitle) {
      window.document.title = newTitle;
    },
    getWidth: function () {
      return window.innerWidth
        || document.documentElement.clientWidth
        || document.body.clientWidth;
		}

  };

  window.Page = page;

})();