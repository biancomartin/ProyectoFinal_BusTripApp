package com.example.domain.response

data class RecorridoBaseInformation(
    val recorridoId: String = "",
    val linea: String= "",
    val coordenadas: List<Coordinates> = emptyList()
)