package com.example.data.response

import com.google.gson.annotations.SerializedName

class PositionRecorridoResponse(
    @SerializedName("recorridoId")
    val recorridoId: String = "",
    @SerializedName("coordenadasIntermedias")
    val coordenadasIntermedias: List<CoordinateResponse> = listOf()
)