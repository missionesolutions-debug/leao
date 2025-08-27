'use strict';

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

var MyUploadAdapter = (function () {
    function MyUploadAdapter(loader) {
        _classCallCheck(this, MyUploadAdapter);

        // The file loader instance to use during the upload. It sounds scary but do not
        // worry — the loader will be passed into the adapter later on in this guide.
        this.loader = loader;
    }

    // Starts the upload process.

    _createClass(MyUploadAdapter, [{
        key: 'upload',
        value: function upload() {
            var _this = this;

            return this.loader.file.then(function (file) {
                return new Promise(function (resolve, reject) {
                    _this._initRequest();
                    _this._initListeners(resolve, reject, file);
                    _this._sendRequest(file);
                });
            });
        }

        // Aborts the upload process.
    }, {
        key: 'abort',
        value: function abort() {
            if (this.xhr) {
                this.xhr.abort();
            }
        }

        // Initializes the XMLHttpRequest object using the URL passed to the constructor.
    }, {
        key: '_initRequest',
        value: function _initRequest() {
            var xhr = this.xhr = new XMLHttpRequest();

            // Note that your request may look different. It is up to you and your editor
            // integration to choose the right communication channel. This example uses
            // a POST request with JSON as a data structure but your configuration
            // could be different.
            xhr.open('POST', '/Imagem/UploadImage', true);
            xhr.responseType = 'json';
        }

        // Initializes XMLHttpRequest listeners.
    }, {
        key: '_initListeners',
        value: function _initListeners(resolve, reject, file) {
            var xhr = this.xhr;
            var loader = this.loader;
            var genericErrorText = 'Couldn\'t upload file: ' + file.name + '.';

            xhr.addEventListener('error', function () {
                return reject(genericErrorText);
            });
            xhr.addEventListener('abort', function () {
                return reject();
            });
            xhr.addEventListener('load', function () {
                var response = xhr.response;

                // This example assumes the XHR server's "response" object will come with
                // an "error" which has its own "message" that can be passed to reject()
                // in the upload promise.
                //
                // Your integration may handle upload errors in a different way so make sure
                // it is done properly. The reject() function must be called when the upload fails.
                if (!response || response.error) {
                    return reject(response && response.error ? response.error.message : genericErrorText);
                }

                // If the upload is successful, resolve the upload promise with an object containing
                // at least the "default" URL, pointing to the image on the server.
                // This URL will be used to display the image in the content. Learn more in the
                // UploadAdapter#upload documentation.
                resolve({
                    'default': response.url
                });
            });

            // Upload progress when it is supported. The file loader has the #uploadTotal and #uploaded
            // properties which are used e.g. to display the upload progress bar in the editor
            // user interface.
            if (xhr.upload) {
                xhr.upload.addEventListener('progress', function (evt) {
                    if (evt.lengthComputable) {
                        loader.uploadTotal = evt.total;
                        loader.uploaded = evt.loaded;
                    }
                });
            }
        }

        // Prepares the data and sends the request.
    }, {
        key: '_sendRequest',
        value: function _sendRequest(file) {
            // Prepare the form data.
            var data = new FormData();

            data.append('upload', file);

            // Important note: This is the right place to implement security mechanisms
            // like authentication and CSRF protection. For instance, you can use
            // XMLHttpRequest.setRequestHeader() to set the request headers containing
            // the CSRF token generated earlier by your application.

            // Send the request.
            this.xhr.send(data);
        }

        // ...
    }]);

    return MyUploadAdapter;
})();

function MyCustomUploadAdapterPlugin(editor) {
    editor.plugins.get('FileRepository').createUploadAdapter = function (loader) {
        // Configure the URL to the upload script in your back-end here!
        return new MyUploadAdapter(loader);
    };
}

ClassicEditor.create(document.querySelector('#Descricao'), {
    extraPlugins: [MyCustomUploadAdapterPlugin]
})['catch'](function (error) {
    console.log(error);
});

ClassicEditor.create(document.querySelector('#NossaHistoria'), {
    extraPlugins: [MyCustomUploadAdapterPlugin]
})['catch'](function (error) {
    console.log(error);
});

ClassicEditor.create(document.querySelector('#FileTags'), {
    extraPlugins: [MyCustomUploadAdapterPlugin]
})['catch'](function (error) {
    console.log(error);
});

ClassicEditor.create(document.querySelector('#Informacoes'), {
    extraPlugins: [MyCustomUploadAdapterPlugin]
})['catch'](function (error) {
    console.log(error);
});

ClassicEditor.create(document.querySelector('#Requesitos'), {
    extraPlugins: [MyCustomUploadAdapterPlugin]
})['catch'](function (error) {
    console.log(error);
});

ClassicEditor.create(document.querySelector('#Atividades'), {
    extraPlugins: [MyCustomUploadAdapterPlugin]
})['catch'](function (error) {
    console.log(error);
});

ClassicEditor.create(document.querySelector('#Beneficios'), {
    extraPlugins: [MyCustomUploadAdapterPlugin]
})['catch'](function (error) {
    console.log(error);
});

