package com.example.domain.usecase

import com.example.domain.response.Coordinates

class InfoPuntoParadaDomain(
    var posicionOrigen: Coordinates,
    var posicionDestino: Coordinates,
    var fecha: String,
    var trayecto: Int,
    var lineaId: Int,
    var unidadId: Int = 1
)

