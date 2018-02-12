app.controller('RTUController', ['$controller', '$scope', function ($controller, $scope) {

    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    var log = console.log;

    $scope.testes = [];

    $scope.init = function () {        
        initEditor();
    };

    $scope.adicionarNovoTesteUnitario = function () {
        $scope.testes.push({});
        setTimeout(initEditor, 10);
    };

    var initEditor = function () {
        $scope.editor = $scope.editor || ContentTools.EditorApp.get();
        $scope.editor.destroy();
        $scope.editor.init('*[data-editable]', 'data-name');
        $scope.editor.addEventListener('saved', editorSaveEvt);
    };

    var editorSaveEvt = function (ev) {

        var regions = ev.detail().regions;
        if (Object.keys(regions).length === 0) {
            return;
        }

        this.busy(true);

        for (var name in regions) {
            if (regions.hasOwnProperty(name)) {
                log(regions[name]);
            }
        }
    };

    $scope.init();
}]);