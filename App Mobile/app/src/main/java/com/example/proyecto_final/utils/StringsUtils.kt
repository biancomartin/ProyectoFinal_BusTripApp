package com.example.proyecto_final.utils

object StringUtils {
    const val EMPTY_STRING: String = ""
}

fun Double.getTimeFormat():String{
    val toMinSeg = (this / 60).toString()

    return "${toMinSeg.substringBefore(".")} min "+"${toMinSeg.substringAfter(".").substring(0,2)} seg"
}