app.controller('RTUController', ['$controller', '$scope', function ($controller, $scope) {

    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    var log = console.log;

    $scope.testes = [];

    $scope.init = function () {

        //$scope.editor = new MediumEditor('.editavel', {
        //    placeholder: { text: 'Digite seu texto aqui ...', hideOnClick: true }
        //});
    };

    $scope.adicionarNovoTesteUnitario = function () {
        $scope.testes.push({});

        //setTimeout(function () {
        //    $scope.editor.destroy();
        //    $scope.editor.addElements('.editavel');
        //    $scope.editor.setup();
        //}, 5);

        //window.e = $scope.editor;
    };

    $scope.init();
}]);