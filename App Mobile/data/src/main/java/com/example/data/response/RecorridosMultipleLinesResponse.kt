package com.example.data.response

import com.google.gson.annotations.SerializedName

class RecorridosMultipleLinesResponse (
    @SerializedName("trayecto")
    val trayecto: String = "",
    @SerializedName("linea")
    val linea: String = "",
    @SerializedName("coordenadasIntermedias")
    val coordenadas: List<CoordinateResponse> = listOf())