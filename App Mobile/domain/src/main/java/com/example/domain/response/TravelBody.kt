package com.example.domain.response

class TravelBody(var tiempo: Double = 0.0,
                 var distancia: Double = 0.0,
                 val linea: String = "",
                 val trayecto: String = "",
                 val coordenadaOrigen: Coordinates = Coordinates(0.0,0.0),
                 val coordenadaDestino: Coordinates = Coordinates(0.0,0.0))