app.service("RTUService", ['$http', function ($http) {

    return {

        /**
        * Obtem RTU de um chamado
        * @param {String} chamado
        * @returns {Promise}
        */
        obterRTU: function (chamado) {
            return $http.get('/RTU/ObterRTU', { data: { Chamado: chamado } });
        },

        /**
        * Salva RTU
        * @param {any} rtu - Objeto com dados do rtu
        * @returns {Promise}
        */
        salvarRTU: function (rtu) {
            return $http.post('/RTU/SalvarRTU', { data: { rtu: rtu } });
        }

    };
}]);