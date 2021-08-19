package com.example.proyecto_final.ui

import com.google.android.gms.maps.model.Marker
import java.io.Serializable

data class Address(
    val placeId: String = "",
    val name: String = "",
    val subText: String = "",
    val marker: Marker? = null,
    var latitude: Double? = 0.0,
    var longitude: Double? = 0.0
) : Serializable