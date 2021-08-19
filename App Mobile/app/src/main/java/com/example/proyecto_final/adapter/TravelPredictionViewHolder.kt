package com.example.proyecto_final.adapter

import android.content.res.ColorStateList
import android.view.View
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.RecyclerView
import com.example.proyecto_final.R
import com.example.proyecto_final.utils.MapUtils
import com.example.proyecto_final.utils.ViewUtils
import com.example.proyecto_final.utils.getTimeFormat
import com.example.domain.response.TravelBody
import com.google.android.gms.maps.model.LatLng
import kotlinx.android.synthetic.main.fragment_ride_data.view.*

class TravelPredictionViewHolder(itemView: View) :
    RecyclerView.ViewHolder(itemView) {

    fun bind(item: TravelBody, algoritmo: String) = with(itemView) {
        val latlngOrigin = LatLng(item.coordenadaOrigen.latitude, item.coordenadaOrigen.longitude)
        val latlngDestino = LatLng(
            item.coordenadaDestino.latitude,
            item.coordenadaDestino.longitude
        )

        itemView.fragment_card_linea_text_value.text = item.linea
        itemView.fragment_card_algorithm_text_value.text = algoritmo
        itemView.fragment_card_time_text_value.text = item.tiempo.getTimeFormat()
        itemView.fragment_card_distance_text_value.text = context.getString(
            R.string.travel_ride_data_distance_text,
            item.distancia.toString().substring(0, 4)
        )
        itemView.fragment_card_recorrido_text_value_origin.text = MapUtils.getAddressByLatLng(
            itemView.context,
            latlngOrigin
        ).name
        itemView.fragment_card_recorrido_text_value_destino.text = MapUtils.getAddressByLatLng(
            itemView.context,
            latlngDestino
        ).name
        itemView.fragment_card_title_text_view_bg.backgroundTintList = getColorTint(item.linea)
        if(item.linea == "502") {
            itemView.fragment_card_linea_text_value.setTextColor(
                ContextCompat.getColor(
                    itemView.context, R.color.colorNegro
                )
            )
        }

    }

    fun getColorTint(linea: String): ColorStateList {
        val states = arrayOf(
            intArrayOf(-android.R.attr.state_enabled),
            intArrayOf(android.R.attr.state_pressed),
            intArrayOf(android.R.attr.state_enabled)
        )

        val colors = intArrayOf(
            ContextCompat.getColor(
                itemView.context, ViewUtils.getBusCard(
                    linea
                )
            ),  // disabled
            ContextCompat.getColor(
                itemView.context, ViewUtils.getBusCard(
                    linea
                )
            ),  // pressed
            ContextCompat.getColor(
                itemView.context, ViewUtils.getBusCard(
                    linea
                )
            ) // enabled
        )
        return ColorStateList(states, colors)
    }

}