app.controller('BaseController', ['$scope', function ($scope) {

    $scope.init = function () {
        $scope.chamado = +$('input[name="chamado"').val();
        setupAlertify();
    };

    var setupAlertify = function () {
        alertify.defaults.transition = 'slide';
        alertify.defaults.theme.ok = 'btn btn-primary';
        alertify.defaults.theme.cancel = 'btn btn-secondary';
        alertify.defaults.theme.input = 'from-control';
        alertify.defaults.glossary.ok = 'OK';
        alertify.defaults.glossary.cancel = 'Cancelar';
        alertify.defaults.glossary.title = '';
    };

    $scope.confirmar = function (titulo, mensagem, cb) {
        alertify.defaults.glossary.title = titulo || 'Confirmação';
        alertify.confirm(mensagem, cb);        
    };

    $scope.erroInsesperado = function () {
        alertify.error("Ocorreu um erro inesperado.");
    };

    $scope.init();

}]);