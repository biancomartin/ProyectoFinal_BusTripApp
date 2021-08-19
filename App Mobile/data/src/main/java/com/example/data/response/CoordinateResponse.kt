package com.example.data.response

import com.google.gson.annotations.SerializedName

class CoordinateResponse (

    @SerializedName("latitude")
    var lat: Double,
    @SerializedName("longitude")
    var lng: Double)