package com.example.domain.response

class TravelLineInformation(
    val trayecto: String = "",
    val linea: String = "",
    val coordenadasIntermedias: List<Coordinates> = listOf()
)