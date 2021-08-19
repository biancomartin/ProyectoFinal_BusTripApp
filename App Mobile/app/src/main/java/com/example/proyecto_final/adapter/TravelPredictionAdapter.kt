package com.example.proyecto_final.adapter

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.example.proyecto_final.R
import com.example.domain.response.TravelBody
import kotlinx.android.synthetic.main.fragment_ride_data.view.*

class TravelPredictionAdapter(
    var items: MutableList<TravelBody> = mutableListOf(),
    var algoritmo: String
) : RecyclerView.Adapter<TravelPredictionViewHolder>() {

    var listener: (() -> Unit)? = null
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int) =
        TravelPredictionViewHolder(
            LayoutInflater.from(parent.context).inflate(
                R.layout.view_travel_prediction_recycler_item,
                parent,
                false
            )
        )

    override fun onBindViewHolder(holder: TravelPredictionViewHolder, position: Int) {
        holder.bind(items[position], algoritmo)
        holder.itemView.fragment_card_image_close.setOnClickListener {
            listener?.invoke()
        }
    }

    override fun getItemCount(): Int = items.size

}