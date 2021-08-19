package com.example.proyecto_final.ui

import com.example.domain.response.Coordinates
import com.example.domain.usecase.InfoPuntoParadaDomain
import java.io.Serializable

class InfoPuntoParada(
    var posicionOrigen: Coordinates,
    var addressOrigen: Address,
    var posicionDestino: Coordinates,
    var addressDestino: Address,
    var fecha: String,
    var trayecto: Int,
    var lineaId: Int,
    var unidadId: Int = 1
) : Serializable

fun InfoPuntoParada.toDomainModel(): InfoPuntoParadaDomain =
    InfoPuntoParadaDomain(
        posicionOrigen = this.posicionOrigen,
        posicionDestino = this.posicionDestino,
        fecha = this.fecha,
        trayecto = this.trayecto,
        lineaId = this.lineaId,
        unidadId = this.unidadId
    )
