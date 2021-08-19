package com.example.data.response

import com.google.gson.annotations.SerializedName

data class ListLineBusResponse (
    @SerializedName("id")
    val id: String = "",
    @SerializedName("base")
    val base: String = "",
    @SerializedName("linea")
    val linea: String = "")