package com.example.data.response

import com.google.gson.annotations.SerializedName

data class TravelBodyBEResponse (

    @SerializedName("tiempo")
    val tiempo: Double = 0.0,
    @SerializedName("distancia")
    val distancia: Double = 0.0,
    @SerializedName("linea")
    val linea: String = "",
    @SerializedName("trayecto")
    val trayecto: String = "",
    @SerializedName("coordenadaOrigen")
    val coordenadaOrigen: CoordinateResponse = CoordinateResponse(0.0,0.0),
    @SerializedName("coordenadaDestino")
    val coordenadaDestino: CoordinateResponse = CoordinateResponse(0.0,0.0)
)