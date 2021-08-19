package com.example.proyecto_final.adapter

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.widget.ImageButton
import android.widget.ImageView
import android.widget.TextView
import androidx.core.content.ContextCompat
import com.example.proyecto_final.R
import com.example.proyecto_final.ui.fragments.MapFragmentViewModel
import com.example.proyecto_final.utils.MapUtils
import com.google.android.gms.maps.GoogleMap.InfoWindowAdapter
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.Marker

class CustomInfoWindowAdapter(
    private val inflater: LayoutInflater,
    private val context: Context,
    private val mapFragmentViewModel: MapFragmentViewModel
) : InfoWindowAdapter {

    var onUserClickListener: ((latLng: LatLng) -> Unit)? = null
    override fun getInfoContents(m: Marker): View {

        val v = inflater.inflate(R.layout.infowindow_layout, null)

        //necesario para frenar el thread cuando muestro un recorrido
        mapFragmentViewModel.userAwaitBusRoutes = true
        mapFragmentViewModel.checkLocation = false

        val address =
            MapUtils.getAddress(context, m)
        (v.findViewById<View>(R.id.info_window_nombre) as TextView).text = TITLE
        (v.findViewById<View>(R.id.info_window_address) as TextView).text = address.name
        (v.findViewById<View>(R.id.info_window_lat) as TextView).text =
            "LAT: " + address.marker?.position?.latitude
        (v.findViewById<View>(R.id.info_window_lng) as TextView).text =
            "LNG: " + address.marker?.position?.longitude
        (v.findViewById<View>(R.id.info_window_image) as ImageView).background =
            ContextCompat.getDrawable(v.context, R.drawable.ic_ubicacion_color)
        (v.findViewById<View>(R.id.info_window_parada_button) as ImageButton).background =
            ContextCompat.getDrawable(v.context, R.drawable.ic_parada_de_autobus)
        (v.findViewById<View>(R.id.info_window_parada_button) as ImageButton).setOnClickListener {
            onUserClickListener?.invoke(m.position)
        }
        return v
    }

    override fun getInfoWindow(m: Marker): View? {
        return null
    }

    companion object {
        private const val TITLE = "Place"
    }

}