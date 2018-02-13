app.controller('RTUController', ['$controller', '$scope', function ($controller, $scope) {

    angular.extend(this, $controller('BaseController', { $scope: $scope }));

    $scope.testes = [];

    $scope.init = function () {
        bindSortable();
    };

    $scope.adicionarNovoTesteUnitario = function () {
        $scope.testes.push({});
        bindSortable();
    };

    $scope.removerTesteUnitario = function (teste) {
        $scope.testes.splice($scope.testes.indexOf(teste), 1);
        bindSortable();
    };

    var bindSortable = function () {

        var sortble = $('.sortable');
        if (sortble.data('uiSortable')) {
            sortble.sortable("destroy");
        }
        sortble.sortable({ handle: '.sortable-handle' });
    };

    $scope.init();
}]);