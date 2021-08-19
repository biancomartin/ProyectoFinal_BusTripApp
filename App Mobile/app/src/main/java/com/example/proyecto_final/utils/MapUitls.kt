package com.example.proyecto_final.utils

import android.content.Context
import android.location.Address
import android.location.Geocoder
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.Marker
import java.util.*

object MapUtils {

    fun getAddress(context: Context, marker: Marker): com.example.proyecto_final.ui.Address {

        val direccion: List<Address>
        val geocoder: Geocoder = Geocoder(context, Locale.getDefault())

        direccion = geocoder.getFromLocation(
            marker.position.latitude,
            marker.position.longitude,
            1
        ) // 1 representa la cantidad de resultados a obtener
        val address = direccion[0].getAddressLine(0)
        val nameAddress = address.split(",")[0]
        val subtextAddressss = address.replace("$nameAddress, ", "")
        return com.example.proyecto_final.ui.Address(
            "",
            nameAddress,
            subtextAddressss,
            marker,
            marker.position.latitude,
            marker.position.longitude
        )
    }

    fun getAddressByLatLng(context: Context, marker: LatLng): com.example.proyecto_final.ui.Address {

        val direccion: List<Address>
        val geocoder: Geocoder = Geocoder(context, Locale.getDefault())

        direccion = geocoder.getFromLocation(
            marker.latitude,
            marker.longitude,
            1
        ) // 1 representa la cantidad de resultados a obtener
        val address = direccion[0].getAddressLine(0)
        val nameAddress = address.split(",")[0]
        val subtextAddressss = address.replace("$nameAddress, ", "")
        return com.example.proyecto_final.ui.Address(
            "",
            nameAddress,
            subtextAddressss,
            null,
            marker.latitude,
            marker.longitude
        )
    }
}