app.controller('BaseController', ['$scope', function ($scope) {

    $scope.init = function () {
        $scope.chamado = +$('input[name="chamado"').val();
    };

    $scope.init();

}]);