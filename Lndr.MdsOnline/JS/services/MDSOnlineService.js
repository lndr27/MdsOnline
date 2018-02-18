﻿/**
* Servico utiizado para comunicacao do front-end (angular) com backend .NET MVC
*/
app.service("MDSOnlineService", ['$http', function ($http) {

    return {

        /**
        * Obtem RTU de um chamado
        * @param {String} chamado
        * @returns {Promise}
        */
        obterRTU: function (chamado) {
            return $http.get('/RTU/ObterRTU', { params: { chamado: chamado } });
        },

        /**
        * Salva testes do RTU
        * @param {any} rtu - Objeto com lista e testes e ID do chamado ex: { Chamado: 123, Testes: [] }
        * @returns {Promise}
        */
        salvarRTU: function (rtu) {
            return $http.post('/RTU/SalvarRTU', { model: rtu });
        }

    };
}]);