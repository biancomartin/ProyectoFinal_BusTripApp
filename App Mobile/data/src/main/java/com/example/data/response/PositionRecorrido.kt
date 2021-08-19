package com.example.data.response

import com.example.domain.response.Coordinates

class PositionRecorrido(
    var coordenadaOrigen: Coordinates,
    var coordenadaDestino: Coordinates,
    var linea: Int
)