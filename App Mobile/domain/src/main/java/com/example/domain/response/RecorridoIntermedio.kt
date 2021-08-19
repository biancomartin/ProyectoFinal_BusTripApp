package com.example.domain.response

data class RecorridoIntermedio(
    val recorridoId: String = "",
    val coordenadasIntermedias: List<Coordinates>)