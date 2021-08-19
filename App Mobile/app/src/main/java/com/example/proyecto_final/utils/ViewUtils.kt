package com.example.proyecto_final.utils

import android.content.Context
import android.graphics.Bitmap
import android.graphics.Canvas
import android.graphics.Rect
import android.os.Handler
import android.util.DisplayMetrics
import android.view.TouchDelegate
import android.view.View
import androidx.core.content.ContextCompat
import com.example.proyecto_final.R
import com.google.android.gms.maps.model.BitmapDescriptor
import com.google.android.gms.maps.model.BitmapDescriptorFactory


object ViewUtils {
    fun bitmapDescriptorFromVector(
        context: Context,
        vectorResId: Int
    ): BitmapDescriptor {
        val vectorDrawable =
            ContextCompat.getDrawable(context, vectorResId)
        vectorDrawable?.setBounds(
            0,
            0,
            vectorDrawable.intrinsicWidth,
            vectorDrawable.intrinsicHeight
        )
        val bitmap = vectorDrawable?.intrinsicHeight?.let {
            Bitmap.createBitmap(
                vectorDrawable.intrinsicWidth,
                it,
                Bitmap.Config.ARGB_8888
            )
        }
        val canvas = bitmap?.let { Canvas(it) }
        canvas?.let { vectorDrawable.draw(it) }
        return BitmapDescriptorFactory.fromBitmap(bitmap)
    }

    fun getBusIcon(line: String): Int =
        when (line) {
            "500" -> {
                R.drawable.ic_autobus_yellow
            }
            "501" -> {
                R.drawable.ic_autobus
            }
            "502" -> {
                R.drawable.ic_autobus_502
            }
            "503" -> {
                R.drawable.ic_autobus_blue
            }
            "504" -> {
                R.drawable.ic_autobus_green
            }
            "505" -> {
                R.drawable.ic_autobus_marron
            }
            else -> {
                R.drawable.ic_autobus_502
            }
        }

    fun getBusColorRoute(line: String): Int =
        when (line) {
            "500" -> R.color.colorYell
            "501" -> R.color.colorRed
            "502" -> R.color.color502
            "503" -> R.color.colorGreenPressed
            "504" -> R.color.colorBlue
            "505" -> R.color.color505
            else -> R.color.colorNegro
        }

    fun getBusCard(line: String): Int =
        when (line) {
            "500" -> R.color.colorYell
            "501" -> R.color.colorRed
            "502" -> R.color.color502Card
            "503" -> R.color.colorGreenPressed
            "504" -> R.color.colorBlue
            "505" -> R.color.color505
            else -> R.color.colorNegro
        }

    fun expandTouchArea(
        context: Context,
        parentView: View,
        view: View,
        extraLeftDp: Int,
        extraRightDp: Int,
        extraTopDp: Int,
        extraBottomDp: Int
    ) {
        parentView.post {
            try {
                val rect = Rect()
                view.getHitRect(rect)
                rect.left -= convertDpToPx(
                    extraLeftDp.toFloat(),
                    context
                ) as Int
                rect.top -= convertDpToPx(
                    extraTopDp.toFloat(),
                    context
                ) as Int
                rect.right += convertDpToPx(
                    extraRightDp.toFloat(),
                    context
                ) as Int
                rect.bottom += convertDpToPx(
                    extraBottomDp.toFloat(),
                    context
                ) as Int
                parentView.touchDelegate = TouchDelegate(rect, view)
            } catch (e: Exception) {
                e.printStackTrace()
            }
        }
    }

    fun convertDpToPx(dp: Float, context: Context): Float {
        val resources = context.resources
        val metrics = resources.displayMetrics
        return dp * (metrics.densityDpi.toFloat() / DisplayMetrics.DENSITY_DEFAULT)
    }
}

const val DEFAULT_DEBOUNCE_TIME = 1500L

fun View.onClickThrottled(onClickAction: () -> Unit, debounceTime: Long = DEFAULT_DEBOUNCE_TIME) {
    var isClickEnable = true
    this.setOnClickListener {
        if (isClickEnable) {
            isClickEnable = false
            onClickAction.invoke()
            Handler().postDelayed({ isClickEnable = true }, debounceTime)
        }
    }
}

