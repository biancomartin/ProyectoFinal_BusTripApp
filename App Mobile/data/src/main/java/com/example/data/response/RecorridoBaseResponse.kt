package com.example.data.response

import com.google.gson.annotations.SerializedName

data class RecorridoBaseResponse(
    @SerializedName("trayecto")
    val recorridoId: String = "",
    @SerializedName("linea")
    val linea: String = "",
    @SerializedName("coordenadas")
    val coordenadas: List<CoordinateResponse> = listOf()
)