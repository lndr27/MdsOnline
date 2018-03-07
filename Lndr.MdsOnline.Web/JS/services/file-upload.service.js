app.service('FileUploadService', ['Upload', '$http', function (Upload, $http) {
    
    return {

        upload: function (file, isRascunho) {
            var url = isRascunho === true ? '/FileApi/UploadRascunho' : '/FileApi/UploadArquivo';
            return Upload.upload({ url: url, data: { file: file } });
        },

        delete: function (guid) {
            return $http.delete('/FileApi/Delete', { params: { guid: guid } });
        }
    };
}]);